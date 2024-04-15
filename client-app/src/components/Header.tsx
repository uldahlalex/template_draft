import React from "react";
import {useNavigate} from "react-router-dom";
import {useAtom} from "jotai/index";
import toast from "react-hot-toast";
import {userAtom, themeAtom, jwtAtom} from "../reusables/state/states.ts";

export default function Header() {

    const [user, setUser] = useAtom(userAtom);
    const [selectedTheme, setSelectedTheme] = useAtom(themeAtom);
    const [jwt, setJwt] = useAtom(jwtAtom);

    const navigate = useNavigate();

    const themes = [
        "light",
        "dark",
        "cupcake",
        "bumblebee",
        "emerald",
        "corporate",
        "synthwave",
        "retro",
        "cyberpunk",
        "valentine",
        "halloween",
        "garden",
        "forest",
        "aqua",
        "lofi",
        "pastel",
        "fantasy",
        "wireframe",
        "black",
        "luxury",
        "dracula",
        "cmyk",
        "autumn",
        "business",
        "acid",
        "lemonade",
        "night",
        "coffee",
        "winter",
        "dim",
        "nord",
        "sunset",
    ];

    function authHeaderTab() {
        return <details className="dropdown dropdown-end">
            <summary className="m-1 btn ">Signed in as: {user!.username}</summary>
            <div
                className="p-2 shadow menu dropdown-content z-[1] bg-base-100 rounded-box w-52 overflow-x-hidden overflow-y-auto max-h-60">
                <button onClick={e => {
                    setJwt('');

                }} className="btn">Sign out</button>
            </div>
        </details>;
    }


    return (<>
        <div className="navbar bg-base-200 mb-5">
            <div className="navbar-start">
                <div className="dropdown">
                    <div tabIndex={0} role="button" className="btn btn-ghost">
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24"
                             stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2"
                                  d="M4 6h16M4 12h16M4 18h7"/>
                        </svg>
                    </div>
                    <ul tabIndex={0}
                        className="menu menu-sm dropdown-content mt-3 z-[1] p-2 shadow bg-base-100 rounded-box w-52">
                        <li><a onClick={() => {
                            navigate('feed');
                        }}>Demo to-do app for Programming II 2024</a></li>
                           <li><a onClick={() => {
                                navigate('login');
                            }}>Go To Login</a></li>
                    </ul>
                </div>
            </div>
            <div className="navbar-center">
                <a className="btn btn-ghost text-xl">Tick</a>
            </div>
            <div className="navbar-end">


                <div className="flex items-stretch">

                    {jwt && authHeaderTab()}

                    <details className="dropdown dropdown-end">
                        <summary className="m-1 btn">Theme</summary>
                        <ul onClick={(e: React.MouseEvent<HTMLElement>) => {
                            const selectedTheme = (e.target as HTMLElement).innerText;
                            setSelectedTheme(selectedTheme);
                        }}
                            className="p-2 shadow menu dropdown-content z-[1] bg-base-100 rounded-box w-96 overflow-x-hidden overflow-y-auto max-h-80">
                            {
                                themes.map((theme, index) => <li key={index}><a>{theme == selectedTheme ? '👉'+theme : theme}</a></li>)
                            }
                        </ul>
                    </details>
                </div>


            </div>
        </div>

    </>)
}