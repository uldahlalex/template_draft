import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {jwtAtom, userAtom} from "../state/atoms/user.ts";

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

    return null; // This component does not render anything
}