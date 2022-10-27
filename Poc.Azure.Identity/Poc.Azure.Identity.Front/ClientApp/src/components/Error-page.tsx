import React from "react";
import {useRouteError} from "react-router-dom";

export const ErrorPage = () => {
    console.error(useRouteError());
    return <div className="component">
        <h1>Error page</h1>
    </div>;
} 