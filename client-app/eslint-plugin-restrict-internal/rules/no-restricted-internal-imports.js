module.exports = {
    meta: {
        type: 'problem',
        docs: {
            description: 'Disallow importing files named internal.*',
            category: 'Possible Errors',
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
                    const lastPart = parts[parts.length - 1];

                    // Check if the imported file is named internal.*
                    if (lastPart.startsWith('internal.')) {
                        context.report({
                            node,
                            message: 'Importing files named internal.* is not allowed',
                        });
                    }
                }
            },
        };
    },
};