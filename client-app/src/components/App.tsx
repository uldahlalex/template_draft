import React from 'react';
import {BrowserRouter as Router, Navigate, Route, Routes} from 'react-router-dom';
import {Toaster} from "react-hot-toast";
import GoToLoginWhenJwtAtomIsNull from "./effects/NavigationEffects/GoToLoginWhenJwtAtomIsNull.tsx";
import HttpErrorInterceptor from "./effects/CommunicationEffects/HttpErrorInterceptor.tsx";
import HttpTokenSetterInterceptor from "./effects/CommunicationEffects/HttpTokenSetterInterceptor.tsx";
import Login from "./Routes/Login/Login.tsx";
import Feed from "./Routes/Feed/Feed.tsx";
import SignInEffect from "./effects/NavigationEffects/SignInEffect.tsx";
import ThemeChange from "./effects/UIEffects/ThemeChange.tsx";
import Header from "./Header.tsx";

export default function App() {

    return (
        <Router>
            <Toaster />
            <GoToLoginWhenJwtAtomIsNull />
            <HttpTokenSetterInterceptor />
            <HttpErrorInterceptor />
            <SignInEffect />
            <ThemeChange />
            <Header />
            <Routes>
                <Route path="/" element={<Navigate to='/feed' replace />} />
                <Route path="/feed" element={<Feed />} />
                <Route path="/login" element={<Login />} />
            </Routes>
        </Router>
    );
}