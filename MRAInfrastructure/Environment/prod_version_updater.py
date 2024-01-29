import os

def update():
    with open(os.path.join("Staging", "kustomization.yml"), "r") as sFile:
        sData = sFile.readlines()

    with open(os.path.join("Production", "kustomization.yml"), "r") as pFile:
        pData = pFile.readlines()

    with open (os.path.join("Production", "kustomization.yml"), "w") as file:
        pData[4] = sData[4]
        pData[6] = sData[6]
        file.writelines(pData)


if __name__ == "__main__":
    update()

