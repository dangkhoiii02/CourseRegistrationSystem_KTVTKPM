import os
import re

def refactor_file(filepath):
    with open(filepath, 'r') as f:
        lines = f.readlines()

    namespace_found = False
    new_lines = []
    
    for line in lines:
        match = re.match(r'^namespace\s+([^;]+);', line)
        if match and not namespace_found:
            namespace_found = True
            new_lines.append(f"namespace {match.group(1)}\n")
            new_lines.append("{\n")
            continue
            
        if namespace_found:
            if line.strip() == "":
                new_lines.append(line)
            else:
                new_lines.append("    " + line)
        else:
            new_lines.append(line)
            
    if namespace_found:
        new_lines.append("}\n")
        
    with open(filepath, 'w') as f:
        f.writelines(new_lines)
        print(f"Refactored: {filepath}")

def process_directory(directory):
    for root, dirs, files in os.walk(directory):
        if 'obj' in root or 'bin' in root or 'Migrations' in root or 'Chuong 4' in root:
            continue
        for file in files:
            if file.endswith('.cs') and file != "Program.cs":
                filepath = os.path.join(root, file)
                refactor_file(filepath)

if __name__ == "__main__":
    process_directory("/Users/dangkhoii/CourseRegistrationSystem")
