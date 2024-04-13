import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {userAtom, jwtAtom} from "../../reusables/state/external.ts";
import {decodeJwt} from "../../reusables/logic/external.ts";


//todo can this be extended to replace effects guards?
//todo make one bigger effect that handles effects state changes?
export default function NavigationEffect() {
    const [jwt, setJwt] = useAtom(jwtAtom);
    const [, setUser ] = useAtom(userAtom);
    const navigate = useNavigate();

    useEffect(() => {
        if (!jwt) {
            setUser(null);
            navigate('/login');
        } else {
            navigate('/feed')
        }
    }, [jwt, setJwt, navigate]);

    return null;
}