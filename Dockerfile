FROM node:22-bookworm


WORKDIR /workspace


# Install useful development tools
RUN apt-get update && \
    apt-get install -y git curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*


# Keep npm available
RUN npm install -g npm@latest


EXPOSE 5173


CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]