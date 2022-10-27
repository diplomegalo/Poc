import React from 'react';
import ReactDOM from 'react-dom/client';
import {Provider} from "react-redux";
import {store} from "./app/store";
import {App} from "./components/App";
import {PublicClientApplication} from "@azure/msal-browser";
import {MsalProvider} from "@azure/msal-react";
import {msalConfig} from "./app/authConfig";

const msalInstance = new PublicClientApplication(msalConfig);

ReactDOM.createRoot(document.getElementById("root") as Element).render(
    <React.StrictMode>
        <MsalProvider instance={msalInstance}>
            <Provider store={store}>
                <App/>
            </Provider>
        </MsalProvider>
    </React.StrictMode>
);
