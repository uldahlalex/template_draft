import React from 'react';
import {BrowserRouter as Router, Navigate, Route, Routes} from 'react-router-dom';
import {Toaster} from "react-hot-toast";
import GoToLoginWhenJwtAtomIsNull from "./effects/NavigationEffects/GoToLoginWhenJwtAtomIsNull.tsx";
import Login from "./Routes/Login/Login.tsx";
import Feed from "./Routes/Feed/Feed.tsx";
import SignInEffect from "./effects/NavigationEffects/SignInEffect.tsx";
import ThemeChange from "./effects/UIEffects/ThemeChange.tsx";
import Header from "./Header.tsx";
import {HttpErrorInterceptor, HttpTokenSetterInterceptor} from "../reusables/logic/external.ts";
import {DevTools} from "jotai-devtools";

export default function App() {

    return (
        <Router>

            {/*Jotai state management (atoms) Dev Tools*/}
            <DevTools/>

            {/*UseEffect*/}
            <GoToLoginWhenJwtAtomIsNull />
            <SignInEffect />
            <ThemeChange />

            {/*The two below are not using useEffect and they also modify state*/}
            <HttpTokenSetterInterceptor />
            <HttpErrorInterceptor />

            {/*UI components*/}
            <Toaster />
            <Header />

            {/*Routes*/}
            <Routes>
                <Route path="/" element={<Navigate to='/feed' replace />} />
                <Route path="/feed" element={<Feed />} />
                <Route path="/login" element={<Login />} />
            </Routes>
        </Router>
    );
}