from cryptography.hazmat.primitives.hashes import *
from cryptography.hazmat.backends import default_backend

backend = default_backend()

def print_hash(f, text):
    digest = Hash(f, backend)
    digest.update(text.encode('utf-8'))
    print(digest.finalize().hex())

texts = [
    'The quick brown fox jumps over the lazy dog',
    'The quick brown fox jumps over the lazy dof',
]

for text in texts:
    print(f'SHAKE-128("{text}", 256):')
    print_hash(SHAKE128(32), text)
