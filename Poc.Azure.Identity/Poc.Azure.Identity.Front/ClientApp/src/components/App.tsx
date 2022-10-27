import React, {useEffect} from "react"
import {createBrowserRouter, redirect, RouterProvider} from "react-router-dom";
import {Root} from "./app/Root";
import {ErrorPage} from "./Error-page";
import {Home} from "./Home";
import {Signup} from "./auth/Signup";
import {MicrosoftSigninButton} from "./auth/MicrosoftSigninButton";
import {useIsAuthenticated} from "@azure/msal-react";

const rootLoader = (isAuthenticate: boolean) => {
    if (!isAuthenticate) {
        return redirect("/signin");
    }
}

export const App = () => {
    // Initiate hooks
    const isAuthenticate = useIsAuthenticated();    
    const router = createBrowserRouter([
        {
            path: "/",
            element: <Root/>,
            loader: () => rootLoader(isAuthenticate),
            errorElement: <ErrorPage/>,
            children: [
                {
                    path: "/home",
                    element: <Home/>
                }
            ]
        },
        {
            path: "/signin",
            element: <MicrosoftSigninButton/>
        },
        {
            path: "/signup",
            element: <Signup/>
        }
    ], {window: window});
    return <RouterProvider router={router}/>;
}
