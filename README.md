# Cyber Kroma

A Unity game development project.

## 🎮 About

Cyber Kroma is a game project developed as part of the Game Development Final Project at CADT.

## 🚀 Getting Started

### Prerequisites

- Unity Hub
- Unity Editor (check `ProjectSettings/ProjectVersion.txt` for the exact version)
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/LK-Hour/Cyber-Kroma.git
cd Cyber-Kroma
```

2. Open Unity Hub and add the project folder

3. Open the project in Unity Editor

## 🤝 Contributing

We welcome contributions from all team members! Follow these steps to contribute:

### Setting Up Your Workspace

1. **Pull the latest changes** before starting work:
```bash
git pull origin main
```

2. **Create a new branch** for your feature:
```bash
git checkout -b feature/your-feature-name
```

### Making Changes

1. Make your changes in Unity or your preferred code editor

2. **Test your changes** in Unity to ensure everything works

3. **Stage your changes**:
```bash
git add .
```

4. **Commit with a clear message**:
```bash
git commit -m "ADD: Description of what you added"
# or
git commit -m "FIX: Description of what you fixed"
# or
git commit -m "UPDATE: Description of what you updated"
```

### Submitting Your Work

1. **Push your branch** to the repository:
```bash
git push origin feature/your-feature-name
```

2. **Create a Pull Request** on GitHub:
   - Go to the repository on GitHub
   - Click "Pull Requests" → "New Pull Request"
   - Select your branch and describe your changes
   - Request a review from team members

3. **Wait for review and approval** before merging

### Working with the Main Branch

If you need to push directly to main (for small fixes):

1. Always pull first to avoid conflicts:
```bash
git pull origin main
```

2. Make your changes and commit

3. Push to main:
```bash
git push origin main
```

### Resolving Conflicts

If you get a merge conflict:

1. Pull the latest changes:
```bash
git pull origin main
```

2. Unity will show conflicted files - resolve them in Unity or a merge tool

3. After resolving, commit the merge:
```bash
git add .
git commit -m "MERGE: Resolved conflicts with main"
git push origin main
```

## 📁 Project Structure

```
Assets/
├── Scenes/          # Unity scene files
├── Scripts/         # C# scripts
├── Prefabs/         # Reusable game objects
├── Materials/       # Materials and shaders
└── Resources/       # Runtime-loaded assets

ProjectSettings/     # Unity project configuration
Packages/           # Unity package dependencies
```

## 👥 Team Members

Each team member has their own scene in `Assets/Scenes/` for individual work.

## 📝 Commit Message Guidelines

Use clear prefixes for commits:
- `ADD:` - New features or files
- `FIX:` - Bug fixes
- `UPDATE:` - Changes to existing features
- `REMOVE:` - Deleted features or files
- `REFACTOR:` - Code improvements without changing functionality
- `DOCS:` - Documentation changes

## ⚠️ Important Notes

- **Never commit the `Library/` folder** - it's automatically generated
- **Test in Unity** before committing to avoid breaking the project
- **Pull before you push** to avoid conflicts
- **Communicate with the team** about major changes
- **Don't force push** (`git push --force`) unless absolutely necessary

## 🆘 Getting Help

If you encounter issues:
1. Check if pulling the latest changes helps: `git pull origin main`
2. Ask team members in the project chat
3. Check Unity console for errors
4. Review this README for contribution guidelines

## 📄 License

This project is for educational purposes as part of CADT coursework.
