// // eslint-plugin-restrict-internal/rules/no-restricted-internal-imports.js
// module.exports = {
//     meta: {
//         type: "problem",
//         docs: {
//             description: "disallow importing from internal directories except from external.ts",
//             category: "Possible Errors",
//             recommended: false,
//         },
//         schema: [], // no options
//     },
//     create: function(context) {
//         return {
//             ImportDeclaration(node) {
//                 const sourceValue = node.source.value;
//                 const internalPattern = /\/internal\//; // Adjust the regex according to your path structure
//                 const allowedFile = /\/external\.ts$/; // Allows importing specifically from external.ts
//
//                 if (internalPattern.test(sourceValue) && !allowedFile.test(sourceValue)) {
//                     context.report({
//                         node,
//                         message: "Imports from internal directories must be from external.ts files only."
//                     });
//                 }
//             }
//         };
//     }
// };


module.exports = {
    meta: {
        type: 'problem',
        docs: {
            description: 'Enforce using external.ts files for exposing internal members',
            category: 'Best Practices',
            recommended: true,
        },
        fixable: null,
        schema: [],
    },
    create: function (context) {
        return {
            ImportDeclaration(node) {
                const importPath = node.source.value;

                // Ignore imports from node_modules
                if (importPath.startsWith('.') || importPath.startsWith('/')) {
                    const parts = importPath.split('/');

                    // Check if the import path contains 'internal'
                    if (parts.includes('internal')) {
                        const lastPart = parts[parts.length - 1];

                        // Check if the import is not from an external.ts file
                        if (lastPart !== 'external.ts') {
                            context.report({
                                node,
                                message: 'Imports from internal directories should be exposed through external.ts files',
                            });
                        }
                    }
                }
            },
        };
    },
};