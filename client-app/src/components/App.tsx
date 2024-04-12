// app.tsx
import React from 'react';
import { BrowserRouter as Router, Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useAtom } from 'jotai';
import {jwtAtom, decodeAndSetUserWhenJwtAtomChanges, User, userAtom, sayHiWhenUserChanges} from '../state/atoms/user';
import {Toaster} from "react-hot-toast";

export default function App() {
    const [user, setUser] = useAtom(userAtom);
    const [jwt, setJwt] = useAtom(jwtAtom);
    useAtom(sayHiWhenUserChanges);
    useAtom(decodeAndSetUserWhenJwtAtomChanges);
    return (
        <Router>
            <Toaster />
            <>
                {JSON.stringify(user, null, 2)}
                {JSON.stringify(jwt, null, 2)}
                <button
                    onClick={() => {
                        const user: User = {
                            username: Math.random().toString(),
                            id: 1,
                        };
                        setUser(user);
                    }}
                >
                    Randomize
                </button>

                <button onClick={() => {
                    setJwt("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJVc2VybmFtZSI6ImJsYWFhaCIsIklkIjoxfQ.1aQtDZb0Vi8tSIt5YGtgEXCtWSh_9asIMLjzFkbwrN2QOGzA4d4kMFo9MtYfTepQ2k5e5PqTGmZt46HmMxKa3A")
                }} className="btn">Randomize jwt</button>
            </>
        </Router>
    );
}