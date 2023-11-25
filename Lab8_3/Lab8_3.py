# Piotr Deda

from cryptography.hazmat.primitives.hashes import *
from cryptography.hazmat.backends import default_backend

backend = default_backend()

def printhash(f, text):
    digest = Hash(f, backend)
    digest.update(text.encode('utf-8'))
    print('Digest is: ' + digest.finalize().hex())

texts = [
    'The quick brown fox jumps over the lazy dog',
    'The quick brown fox jumps over the lazy cog',
    '',
]

class MD4(HashAlgorithm):
    name = 'md4'
    digest_size = 16
    block_size = 64

functions = [
    ('MD4', MD4()),
    ('MD5', MD5()),
    ('SHA-1', SHA1()),
    ('SHA-224', SHA224()),
]

for (dname, f) in functions:
    print()
    print(dname)
    for text in texts:
        printhash(f, text)

print()
print('SHA-256, SHA-384, SHA-512')
printhash(SHA256(), texts[2])
printhash(SHA384(), texts[2])
printhash(SHA512(), texts[2])
