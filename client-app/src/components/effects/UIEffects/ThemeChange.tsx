import {useAtom} from "jotai";
import {themeAtom} from "../../../reusables/state/states.ts";
import {useEffect} from "react";

export default function ThemeChange() {

    const [theme] = useAtom(themeAtom);

    useEffect(() => {
        document.documentElement.setAttribute('data-theme', theme);
    }, [theme]);

    return null;
}