import {useMsal} from "@azure/msal-react";
import {loginRequest} from "../../app/authConfig";
import {AuthService} from "../../services/auth.service";
import {Link, useNavigate} from "react-router-dom";
import React from "react";

export const MicrosoftSigninButton = () => {
    const {instance} = useMsal();
    const navigate = useNavigate();
    
    const handleLogin = (loginType: string) => {
        if (loginType === "popup") {
            instance.loginPopup(loginRequest)
                .then((authenticationResult) => {
                    AuthService.setAccesToken(authenticationResult.accessToken);
                    navigate("/home");
                })
                .catch(e => console.error(e));
        }
    }
    return (
        <>
            <Link to="/home">Home</Link>
            <button onClick={() => handleLogin("popup")}>Sign in with Microsoft</button>
        </>
    );
}