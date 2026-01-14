FROM node:20.11.1
WORKDIR /app

COPY src/frontend/package.json src/frontend/package-lock.json* ./
RUN if [ -f package-lock.json ]; then npm ci; else echo "No lockfile yet"; fi

COPY src/frontend ./

EXPOSE 3000
CMD ["npm", "run", "dev", "--", "-H", "0.0.0.0", "-p", "3000"]
