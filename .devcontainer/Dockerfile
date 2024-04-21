FROM mcr.microsoft.com/dotnet/sdk:8.0

# Install useful tools and dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends git procps

# Clean up
RUN apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory
WORKDIR /workspace

RUN bash -c "$(wget https://raw.githubusercontent.com/ohmybash/oh-my-bash/master/tools/install.sh -O -)"
