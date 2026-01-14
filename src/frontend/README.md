# Frontend

This directory contains the Next.js App Router frontend scaffold.

## Initialization notes

The scaffold mirrors the output of `create-next-app@latest` with TypeScript, Tailwind, ESLint,
App Router, and the `@/*` alias. In this environment, `npx create-next-app` was blocked by an npm
registry policy (HTTP 403). If you have registry access locally, you can re-run the command below
and compare outputs:

```
cd src
npx create-next-app@latest frontend --ts --tailwind --eslint --app --src-dir --import-alias "@/*"
```

After initialization, confirm `next --version` and update the root README if needed.
