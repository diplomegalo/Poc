import React, {useState} from "react";
import {useMsal} from "@azure/msal-react";
import PlopService from "../services/plop.service";
import {loginRequest} from "../app/authConfig";
import {AuthService} from "../services/auth.service";

export const Home = function () {
    const {accounts, instance} = useMsal();
    const [data, setData] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [authCode, setAuthCode] = useState<string>("");

    const name = accounts[0] && accounts[0].name;

    const requestProtectedDataFromApi = async () => {
        setError(null);
        setData(null);
        PlopService.getData()
            .then((result) => {
                setData(result);
            })
            .catch((error) => setError(error.message));
    };
    
    const requestAccessToken = async () =>{
        instance.acquireTokenSilent(loginRequest)
            .then((response) => AuthService.setAccesToken(response.accessToken))
            .catch(() => instance.acquireTokenPopup(loginRequest));
    };
    
    const requestAccessTokenByCode = async () => {

        const tokenRequest = {
            code: authCode,
            scopes: ["user.read", "offline_access"],
            redirectUri: "https://localhost:7001/signin-oidc"
        };
        instance.acquireTokenByCode(tokenRequest)
            .then((response) => console.log(response.accessToken));
    }

    return (
        <>
            <h1 className="card-title">Welcome {name} </h1>
            <button type="button" onClick={requestProtectedDataFromApi}>Request data from API</button>
            <div>
                <div>{data}</div>
                <span>{error}</span>
            </div>
            <div>
                <button type="button" onClick={requestAccessToken}>Require access token</button>
            </div>
            <div>
                <input type={"text"} onChange={(e) => setAuthCode(e.target.value)} />
                <button type="button" onClick={requestAccessTokenByCode}>Require access token</button>
            </div>
        </>
    );
}
