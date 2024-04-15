import {useState} from "react";
import {decodeJwt, http} from "../../../reusables/logic/logic.ts";
import {useAtom} from "jotai";
import {jwtAtom, userAtom} from "../../../reusables/state/states.ts";

export default function Login() {

    const [, setJwt] = useAtom(jwtAtom);
    const [, setUser] = useAtom(userAtom);

    const [authForm, setauthForm] = useState({
        username: "",
        password: ""
    });

    const signIn = (e) => {
        http.api
            .signinCreate(authForm)
            .then((r) => {
                authenticateWithToken(r.data.token!);
            })
    }
    const register = (e) => {
        http.api
            .registerCreate(authForm)
            .then((r) => {
                authenticateWithToken(r.data.token!);
        });
    }

    const authenticateWithToken = (token) => {
        setJwt(token);
    }

    return (
        <div className="flex justify-center">

            <div className="card w-96 glass">
                <figure><img
                    src="https://upload.wikimedia.org/wikipedia/commons/thumb/8/82/Check_mark_9x9.svg/2048px-Check_mark_9x9.svg.png"
                    alt="car!"/>
                </figure>
                <div className="card-body">
                    <h2 className="card-title">Authenticate</h2>

                    <label className="input input-bordered flex items-center gap-2">

                        <input type="text" onChange={e => setauthForm({...authForm, username: e.target.value})} name="username" className="grow"
                               placeholder="email@address.com"/>
                    </label>

                    <label className="input input-bordered flex items-center gap-2">

                        <input onChange={e => setauthForm({...authForm, password: e.target.value})}
                               name="password" type="password" className="grow"
                               onKeyDown={e => e.key === 'Enter' ? signIn(e) : null}
                               placeholder="••••••••"/>
                    </label>

                    <div className="card-actions justify-center">
                        <button className="btn btn-secondary" onClick={register}>Sign up</button>
                        <button className="btn btn-primary" onClick={signIn}>Sign in</button>
                    </div>
                </div>
            </div>

        </div>
    );
}