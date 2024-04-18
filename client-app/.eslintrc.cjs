module.exports = {
    "env": {
        "browser": true,
        "es2021": true
    },
    "extends": [
        "eslint:recommended",
        "plugin:@typescript-eslint/recommended",
        "plugin:react/recommended"
    ],
    "overrides": [
        {
            "env": {
                "node": true
            },
            "files": [
                ".eslintrc.{js,cjs}"
            ],
            "parserOptions": {
                "sourceType": "script"
            }
        },
        {
            "files": ["**/*.ts", "**/*.tsx"],

        }
    ],
    "parser": "@typescript-eslint/parser",
    "parserOptions": {
        "ecmaVersion": "latest",
        "sourceType": "module",
        "project": "./tsconfig.json"
    },
    "plugins": [
        "@typescript-eslint",
        "react",
        "restrict-internal"
    ],
    "rules": {
        "@typescript-eslint/no-unused-vars": "off",
        "react/react-in-jsx-scope": "off",
        "@typescript-eslint/no-explicit-any": "off",
        "react/prop-types": "off",
        "no-console": "warn",
        "react/jsx-key": ["error", {"checkFragmentShorthand": true}],
        "@typescript-eslint/ban-ts-comment": "off",
        "restrict-internal/no-restricted-internal-imports": "error"

    }
};