// Is triggered as soon as the localstorage token is set to a user
import {jwtAtom} from "../../../reusables/state/states.ts";
import {useAtom} from "jotai";
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";

export default function SignInEffect() {
    const [jwt] = useAtom(jwtAtom);
    const navigate = useNavigate();

    useEffect(() => {
        if (jwt) {
            navigate('/feed');
            //how do i set user data from jwt to useratom without changing state inside a useffect?
            toast.success('Successful Authentication')
        }
    }, [jwt]);

    return null;
}