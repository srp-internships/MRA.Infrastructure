
import sys

def update(fileName, image, version):
    with open(fileName, "r") as file:
        data = file.readlines()

    if image == "mra-web-api":
        data[6] = f"  newTag: {version}\n"
    elif image == "mra-identity-api":
        data[4] = f"  newTag: {version}\n"
    elif image == "mra-pages-api":
        data[8] = f"  newTag: {version}\n"

    with open (fileName, "w") as file:
        file.writelines(data)


if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: python update_kustomization.py <file_path> <image_name> <version>")
        sys.exit(1)

    update(sys.argv[1], sys.argv[2], sys.argv[3])

