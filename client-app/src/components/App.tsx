// app.tsx
import React from 'react';
import { BrowserRouter as Router, Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useAtom } from 'jotai';
import {Toaster} from "react-hot-toast";
import NavigationEffect from "./effects/NavigationEffect.tsx";
import {jwtAtom, UserAtom, userAtom} from "../reusables/state/external.ts";
import SetupHttpClient from "./effects/setupHttpClient.tsx";

export default function App() {
    const [user, setUser] = useAtom(userAtom);
    const [jwt, setJwt] = useAtom(jwtAtom);

    return (
        <Router>
            <Toaster />
            <NavigationEffect />
            <SetupHttpClient />
            <>
                USER: {JSON.stringify(user, null, 2)}
                <br />
                <br />
                JWT: {JSON.stringify(jwt, null, 2)}
                <br />
                <br />
                <button className="btn"
                        onClick={() => {
                            const user: UserAtom = {
                                username: Math.random().toString(),
                                id: 1,
                            };
                            setUser(user);
                        }}
                >
                    Randomize
                </button>

                <br/>
                <br/>
                <button onClick={() => {
                    setJwt("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJVc2VybmFtZSI6ImJsYWFhaCIsIklkIjoxfQ.1aQtDZb0Vi8tSIt5YGtgEXCtWSh_9asIMLjzFkbwrN2QOGzA4d4kMFo9MtYfTepQ2k5e5PqTGmZt46HmMxKa3A")
                }} className="btn">Randomize jwt
                </button>
                <button onClick={() => {
                    setJwt('')
                }} className="btn">Clear jwt
                </button>
            </>
        </Router>
    );
}