# Use Ubuntu 24.04 as the base image
FROM ubuntu:24.04

# Avoid warnings by switching to noninteractive
ENV DEBIAN_FRONTEND=noninteractive

# Update the system and install basic tools
RUN apt-get update && apt-get install -y \
    # Install basic command-line utilities
    git \
    curl \
    vim \
    # Install other necessary utilities
    build-essential \
    # Clean up
    && apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*

# Switch back to dialog for any ad-hoc use of apt-get
ENV DEBIAN_FRONTEND=dialog

# Set the working directory
WORKDIR /workspace

# Optionally, you can set the default command for the container
CMD ["bash"]
