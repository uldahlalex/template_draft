import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {jwtAtom} from "../../../reusables/state/external.ts";



export default function GoToLoginWhenJwtAtomIsNull() {
    const [jwt] = useAtom(jwtAtom);
    const navigate = useNavigate();

    useEffect(() => {
        if (!jwt) navigate('/login');
    }, [jwt]);

    return null;
}