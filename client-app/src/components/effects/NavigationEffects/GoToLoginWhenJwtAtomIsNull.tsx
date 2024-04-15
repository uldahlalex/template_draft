import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {jwtAtom} from "../../../reusables/state/states.ts";



export default function GoToLoginWhenJwtAtomIsNull() {
    const [jwt] = useAtom(jwtAtom);
    const navigate = useNavigate();

    //todo merge with other effect for more DRY code or not?
    useEffect(() => {
        if (!jwt) navigate('/login');
    }, [jwt]);

    return null;
}