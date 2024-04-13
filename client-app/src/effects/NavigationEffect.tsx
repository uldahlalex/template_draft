import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {userAtom} from "../atoms/internal/userAtom.ts";
import {jwtAtom} from "../atoms/internal/jwtAtom.ts";


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