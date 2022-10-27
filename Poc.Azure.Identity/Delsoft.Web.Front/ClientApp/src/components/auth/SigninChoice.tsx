import {MicrosoftSigninButton} from "./MicrosoftSigninButton";
import React from "react";
import {Link} from "react-router-dom";


export const SigninChoice = () => {
    <>
        <div>
            <MicrosoftSigninButton />    
        </div>
        <div>
            <Link to="/signup">Signup new user</Link>
        </div>
    </>
}