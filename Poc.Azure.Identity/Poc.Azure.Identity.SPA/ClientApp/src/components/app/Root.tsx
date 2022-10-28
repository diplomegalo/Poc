import React, { SyntheticEvent } from "react";
import { Link, Outlet } from "react-router-dom";
import {MicrosoftSignoutButton} from "../auth/MicrosoftSignoutButton";

export const Root = () =>
{
    return (
        <div className="component">
            <header>
                <span>
                    <nav>
                        <ul>
                            <li>
                                <Link to="/home">Home</Link>
                            </li>
                        </ul>
                    </nav>
                    <MicrosoftSignoutButton />
                </span>
            </header>
            <main>
                <aside></aside>
                <div>
                    <Outlet />
                </div>
            </main>
            <footer></footer>
        </div>
    );
}