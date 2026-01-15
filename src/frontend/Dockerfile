FROM node:25
WORKDIR /app

COPY src/frontend ./
RUN  npm install

EXPOSE 3000
CMD ["npm", "run", "dev", "--", "-H", "0.0.0.0", "-p", "3000"]
