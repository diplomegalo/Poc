type MsalConfig = {
    auth: { clientId: string, authority: string, redirectUri: string}
    cache: { cacheLocation: string, storeAuthStateInCookie: boolean }
}

export const msalConfig: MsalConfig = {
    auth: {
        clientId: "6e3ea41b-8df8-41f8-b3ac-fe7848220df0",
        authority: "https://login.microsoftonline.com/common", // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
        redirectUri: "http://localhost:8080/",
    },
    cache: {
        cacheLocation: "localStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    }
};

// Add scopes here for ID token to be used at Microsoft identity platform endpoints.
export const loginRequest = {
    scopes: ["6e3ea41b-8df8-41f8-b3ac-fe7848220df0/App.Access"]
};

// Add the endpoints here for Microsoft Graph API services you'd like to use.
export const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com"
};