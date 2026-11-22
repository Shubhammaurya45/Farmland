# Copilot Instructions

## Project Guidelines
- In this Unity project, seed and water particle systems are children/components of the Player object; collision target resolution must use the object passed as `other` by `OnParticleCollision`, not the particle system's own parent hierarchy.