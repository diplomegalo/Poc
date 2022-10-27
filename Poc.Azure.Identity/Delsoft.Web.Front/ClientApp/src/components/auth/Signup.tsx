import {ActionFunctionArgs, Form, Link} from "react-router-dom";
import React from "react";
import {Dispatch} from "@reduxjs/toolkit";
import {AuthService} from "../../services/auth.service";
import {register, registerFail} from "../../features/authSlice";

export const signupAction = ({request}: ActionFunctionArgs) => (dispatch: Dispatch) => {
    request.formData().then(
        (data) => {
            const username = data.get("username") as string;
            const email = data.get("email") as string;
            const password = data.get("password") as string;

            AuthService.register(username, email, password)
                .then(() => {
                    dispatch(register);
                }); 
        },
        (error) => {
            dispatch(registerFail);
        });
}

export const Signup = () => {
    return (
        <div className="component">
            <h1>Signup</h1>
            <Form method="post">
                <div>
                    <label htmlFor="user">User:</label>
                    <input type="text" id="user" name="user" placeholder="User"/>
                </div>

                <div>
                    <label htmlFor="email">Email:</label>
                    <input type="text" id="email" name="email" placeholder="Email"/>
                </div>

                <div>
                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" name="password" placeholder="Password"/>
                </div>

                <div>
                    <Link to="/signin">Vous avez déjà un compte</Link>
                    <input type="submit" value="Signup"/>
                </div>
            </Form>
        </div>);
}