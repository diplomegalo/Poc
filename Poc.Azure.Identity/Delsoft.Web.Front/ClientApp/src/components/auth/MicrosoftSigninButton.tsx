import {useIsAuthenticated, useMsal} from "@azure/msal-react";
import {loginRequest} from "../../app/authConfig";
import {useEffect} from "react";

export const MicrosoftSigninButton = () => {
    const {instance} = useMsal();
    const isAuthenticate = useIsAuthenticated();
    
    useEffect(() => console.log("Is Authenticated:", isAuthenticate),
        [isAuthenticate]);
    
    const handleLogin = (loginType: string) => {
        if (loginType === "popup") {
            instance.loginPopup(loginRequest)
                .then((result) => console.log(result))
                .catch(e => console.error(e));
        }
    }
    return <button onClick={() => handleLogin("popup")}>Sign in with Microsoft</button>
}