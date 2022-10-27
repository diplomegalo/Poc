import axios from "axios";

export interface User {
    token: string
}

const API_URL: string = "https://localhost:7001/api/auth/";
const TOKEN_KEY: string = "user";

export class AuthService {
    static login(username: string, password: string): Promise<User> {
        return axios
            .post(`${API_URL}signin`, {username, password})
            .then((response) => {
                if (response.data.token) {

                    // register in local storage
                    localStorage.setItem(TOKEN_KEY, JSON.stringify(response.data))

                    // Set authorization header
                    axios.defaults.headers.common = {
                        "Authorization": `Bearer ${response.data.token}`
                    }
                }

                return {token: response.data.token};
            });
    }

    logout() {
        
        // delete entry in local storage
        localStorage.removeItem(TOKEN_KEY);
        
        // unset authorization header
        axios.defaults.headers.common = {
            "Auhtorization": ""
        }
    }

    static register(username: string, email: string, password: string): Promise<void> {
        return axios.post(`${API_URL}signup`, {username, email, password});
    }
}