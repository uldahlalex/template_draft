// app.tsx
import React from 'react';
import { BrowserRouter as Router, Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { useAtom } from 'jotai';
import {User, userAtom, userEffect} from '../state/atoms/user';

export default function App() {
    const [user, setUser] = useAtom(userAtom);
    useAtom(userEffect);
    return (
        <Router>
            <>
                {JSON.stringify(user, null, 2)}
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
            </>
        </Router>
    );
}