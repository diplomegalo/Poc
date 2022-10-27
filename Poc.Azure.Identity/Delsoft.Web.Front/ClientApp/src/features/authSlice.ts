import {createSlice, PayloadAction} from '@reduxjs/toolkit'

export interface AuthenticateState {
    token?: string
    isAuthenticate: boolean
    username?: string
    email?: string
}

const initialState: AuthenticateState = {
    token: undefined,
    isAuthenticate: false,
    username: undefined,
    email: undefined
}

export const authenticateSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        register: (state) => {
            state.isAuthenticate = false;
        },
        registerFail: (state) => {
            state.isAuthenticate = false;
        },
        login:(state) => {
            state.isAuthenticate = true;
        },
        loginFail:(state)=>{
            state.isAuthenticate = false
            state.token = undefined;
            state.username = undefined;
            state.email = undefined;
        },
        logout:(state)=>{
            state.isAuthenticate = false
            state.token = undefined;
            state.username = undefined;
            state.email = undefined;
        }
    }
})

export const {register, registerFail, login, loginFail, logout} = authenticateSlice.actions
export default authenticateSlice.reducer
