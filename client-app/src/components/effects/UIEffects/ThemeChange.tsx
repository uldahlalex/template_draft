import {useAtom} from "jotai";
import {useEffect} from "react";
import {themeAtom} from "../../../reusables/state/states.ts";

export default function ThemeChange() {

    const [theme] = useAtom(themeAtom);

    useEffect(() => {
        document.documentElement.setAttribute('data-theme', theme);
    }, [theme]);

    return null;
}