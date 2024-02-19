from cryptography.hazmat.primitives.hashes import *
from cryptography.hazmat.backends import default_backend

backend = default_backend()

def print_hash(f, text):
    digest = Hash(f, backend)
    digest.update(text.encode('utf-8'))
    print(digest.finalize().hex())

text = ''

functions = [
    ('SHA-3-224("@"):', SHA3_224()),
    ('SHA-3-256("@"):', SHA3_256()),
    ('SHA-3-384("@"):', SHA3_384()),
    ('SHA-3-512("@"):', SHA3_512()),
    ('SHAKE-128("@", 256):', SHAKE128(32)),
    ('SHAKE-128("@", 512):', SHAKE128(64)),
    ('SHAKE-256("@", 512):', SHAKE256(64)),
]

for (dname, f) in functions:
    print(dname.replace('@', text))
    print_hash(f, text)
