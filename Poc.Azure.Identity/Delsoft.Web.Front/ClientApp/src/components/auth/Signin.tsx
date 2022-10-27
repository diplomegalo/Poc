import React, { useEffect } from "react";
import { Link, Navigate, useNavigate } from "react-router-dom";
import { login } from "../../features/authSlice";
import { AuthService } from "../../services/auth.service";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import {MicrosoftSigninButton} from "./MicrosoftSigninButton";

export const Signin = () =>
{
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const {isAuthenticate} = useAppSelector(state => state.authenticate);
    
    useEffect(() =>
    {
        navigate( "/home", {replace: true});
    }, [isAuthenticate]);

    
    const handleSubmit = (e: React.SyntheticEvent) =>
    {
        e.preventDefault();        
        const target = e.target as typeof e.target & {
            email: { value: string },
            password: { value: string }
        };

        const email = target.email.value;
        const password = target.password.value;

        AuthService.login(email, password).then((token) => token && dispatch(login()));
    }

    return (
        <div className="component">
            {isAuthenticate && <Navigate to="/home" replace={true}/>}
            <h1>Signin</h1>
            <form method="post" onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <input type="text" id="email" name="email" placeholder="Email" />
                </div>

                <div>
                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" name="password" placeholder="Your password" />
                </div>

                <div>
                    <input type="submit" value="Login" />
                </div>
            </form>

        </div>);
}