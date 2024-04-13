import React from 'react';
import {BrowserRouter as Router} from 'react-router-dom';
import {Toaster} from "react-hot-toast";
import NavigationEffect from "./effects/NavigationEffect.tsx";
import SetupHttpClient from "./effects/setupHttpClient.tsx";
import DataDisplay from "./DataDisplay.tsx";

export default function App() {

    return (
        <Router>
            <Toaster />
            <NavigationEffect />
            <SetupHttpClient />
        <DataDisplay />
        </Router>
    );
}