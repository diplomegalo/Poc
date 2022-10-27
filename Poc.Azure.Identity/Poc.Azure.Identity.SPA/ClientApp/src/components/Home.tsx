import React, {useState} from "react";
import {useMsal} from "@azure/msal-react";
import {loginRequest} from "../app/authConfig";
import PlopService from "../services/plop.service";

export const Home = function () {
    const {instance, accounts} = useMsal();
    const [accessToken, setAccessToken] = useState<string | null>(null);
    const [data, setData] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    
    const name = accounts[0] && accounts[0].name;

    const requestAccessToken = () => {
        const request = {...loginRequest, account: accounts[0]};

        instance.acquireTokenSilent(request)
            .then((response) => setAccessToken(response.accessToken))
            .catch(() => {
                instance.acquireTokenPopup(request)
                    .then((response) => setAccessToken(response.accessToken));
            });
    }

    const requestProtectedDataFromApi = async () => {
        await PlopService.getData(accessToken)
            .then((result) => {
                setError(null);
                setData(result);
            })
            .catch((error) => setError(error.message));
    }

    return (
        <>
            <h1 className="card-title">Welcome {name} </h1>
            <div>
                {accessToken
                    ? <div><p>Access Token Acquired!</p></div>
                    : <button type="button" onClick={requestAccessToken}>Request Access Token</button>
                }
            </div>

            <div>
                {data
                    ? <div>{data}</div>
                    : <button type="button" onClick={requestProtectedDataFromApi}>Request data from API</button>
                }
                <span>{error}</span>
            </div>
        </>
    );
}
