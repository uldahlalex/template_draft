import {useAtom} from "jotai/index";
import {
    decodeAndSetUserWhenJwtAtomChanges,
    getBackToLoginWhenJwtIsRemoved,
    sayHiWhenUserChanges
} from "../state/atoms/user.ts";

export default function RegisterAtomEffects() {
    useAtom(sayHiWhenUserChanges);
    useAtom(decodeAndSetUserWhenJwtAtomChanges);
    useAtom(getBackToLoginWhenJwtIsRemoved);
    return <></>
}