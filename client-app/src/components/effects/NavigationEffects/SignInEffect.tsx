
// Is triggered as soon as the localstorage token is set to a user
import {jwtAtom, User, userAtom} from "../../../reusables/state/external.ts";
import {useAtom} from "jotai";
import {useEffect} from "react";
import {decodeJwt} from "../../../reusables/logic/external.ts";
import {useNavigate} from "react-router-dom";
import toast from "react-hot-toast";

export default function SignInEffect() {
    const [jwt] = useAtom(jwtAtom);
    const [, setUser] = useAtom(userAtom);
    const navigate = useNavigate();

    //todo this is doing a lot
    useEffect(() => {
        if(jwt) {
            const decoded = decodeJwt(jwt);
            if(decoded.Id == undefined || decoded.Username == undefined)  {
                throw new Error('Invalid token');
            }
            const newUser: User = {
                id: decoded.Id,
                username: decoded.Username
            }
            setUser(newUser);
            navigate('/feed');
            toast.success('Welcome back ' + newUser.username + '!')
        }
    }, [jwt]);

    return null;
}