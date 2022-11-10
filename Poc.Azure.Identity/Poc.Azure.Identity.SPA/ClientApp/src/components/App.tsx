import {createBrowserRouter, redirect, RouterProvider} from "react-router-dom";
import {Root} from "./app/Root";
import {ErrorPage} from "./Error-page";
import {Home} from "./Home";
import {MicrosoftSigninButton} from "./auth/MicrosoftSigninButton";
import {useIsAuthenticated} from "@azure/msal-react";
import {useEffect} from "react";

const rootLoader = (isAuthenticate: boolean) => {
    // if (!isAuthenticate) {
    //     return redirect("/signin");
    // }
}

export const App = () => {
    // Initiate hooks
    const isAuthenticated = useIsAuthenticated();
    
    useEffect(() => console.log("Is authenticated:", isAuthenticated), 
        [isAuthenticated]);
    
    const router = createBrowserRouter([
        {
            path: "/",
            element: <Root/>,
            loader: () => rootLoader(isAuthenticated),
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
        }
    ], {window: window});
    return <RouterProvider router={router}/>;
}
