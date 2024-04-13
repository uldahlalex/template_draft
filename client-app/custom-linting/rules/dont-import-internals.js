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