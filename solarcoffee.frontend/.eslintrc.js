module.exports = {
	root: true,
	env: {
		node: true,
	},
	extends: [
		"plugin:vue/essential",
		"eslint:recommended",
		"@vue/typescript/recommended",
		"@vue/prettier",
		"prettier/@typescript-eslint",
	],
	parser: "vue-eslint-parser",
	parserOptions: {
		parser: "@typescript-eslint/parser",
		ecmaVersion: 2020,
	},
	rules: {
		"no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
		"no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
		"@typescript-eslint/interface-name-prefix": "off",
		"prettier/prettier": ["warn", { endOfLine: "auto" }]
	},
};
