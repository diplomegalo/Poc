import {configureStore} from "@reduxjs/toolkit";
import authenticateReducer from "../features/authSlice";

export const store = configureStore({
    reducer: {
        authenticate:  authenticateReducer
    }
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
