#!/bin/bash

# Define the path to the diagrams directory
diagrams_dir="./diagrams"
combined_file="${diagrams_dir}/combined.puml"

# Check if the diagrams directory exists
if [ ! -d "$diagrams_dir" ]; then
  echo "The directory ${diagrams_dir} does not exist."
  exit 1
fi

# Create or clear the content of the combined.puml file
echo "@startuml" > "$combined_file"

# Loop through all .puml files in the diagrams directory and append their content
for file in ${diagrams_dir}/*.puml; do
    if [ "$file" != "$combined_file" ]; then
        # Append the contents of the .puml file to combined.puml, excluding the start and end tags
        grep -v -e "@startuml" -e "@enduml" "$file" >> "$combined_file"
    fi
done

# Finish the combined.puml file
echo "@enduml" >> "$combined_file"

echo "Combined file created at ${combined_file}"
