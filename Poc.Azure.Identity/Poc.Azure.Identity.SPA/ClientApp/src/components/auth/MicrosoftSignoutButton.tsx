import {useMsal} from "@azure/msal-react";

export const MicrosoftSignoutButton = () => {
    const {instance} = useMsal();
    
    const handleLogin = (logoutType: string) => {
        if (logoutType === "popup") {
            instance.logoutPopup({
                postLogoutRedirectUri: "/",
                mainWindowRedirectUri: "/"
            })
        }
    }
    return <button onClick={() => handleLogin("popup")}>Sign out with Microsoft</button>
}